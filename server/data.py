from pymongo import MongoClient, errors
from bson.objectid import ObjectId
import helpers
import gridfs
import datetime
import requests
from mpns import MPNSToast, MPNSTile

class DataBase:
    def __init__(self, db_name):
        self.db_name = db_name
        self.db = MongoClient()[db_name]
        self.fs = gridfs.GridFS(self.db)

    def drop_data_base(self):
        MongoClient().drop_database(self.db_name)

    def create_customer(self, name, email, password):
        self.db.customers.ensure_index("email", unique=True)
        if not helpers.email_is_valid(email) or not helpers.name_is_valid(name) or not helpers.password_is_valid(password):
            return False
        encrypted_password = helpers.encrypt_password(password)
        customer_doc = {
            "name": name,
            "email": email,
            "password": encrypted_password
        }
        try:
            result = self.db.customers.insert(customer_doc)
        except errors.DuplicateKeyError:
            result = False
        return result

    def get_customer(self, customer_id):
        return self.db.customers.find_one(ObjectId(customer_id))

    def get_auction(self, auction_id):
        return self.db.auctions.find_one(ObjectId(auction_id))

    def login(self, email, password):
        encrypted_password = helpers.encrypt_password(password)
        doc = {
            "email": email,
            "password": encrypted_password
        }
        return self.db.customers.find_one(doc)

    def create_auction(self, name, minimum_bid, photo):
        photo_id = str(ObjectId())
        photo.save("images/" + photo_id)
        auction_doc = {
            "name": name,
            "minimum_bid": minimum_bid,
            "photo_id": photo_id,
            "state": "open",
            "date": datetime.datetime.utcnow(),
            "bids": []
        }
        return self.db.auctions.insert(auction_doc)

    def get_auctions(self):
        cursor = self.db.auctions.find()
        results = []
        for doc in cursor:
            doc["id"] = str(doc["_id"])
            doc["photo_id"] = str(doc["photo_id"])
            doc["date"] = helpers.format_date(doc["date"])
            del doc["_id"]
            results.append(doc)
        return results

    def create_bid(self, value, customer_id, auction_id):
        auction = self.db.auctions.find_one(ObjectId(auction_id))

        if auction and auction["state"] == "open":
            if len(auction["bids"]) > 0:
                bids_size = len(auction["bids"])
                if auction["bids"][bids_size-1]["value"] < value and value > auction["minimum_bid"]:
                    bid_doc = {
                        "value": float(value),
                        "date": datetime.datetime.utcnow(),
                        "customerID": ObjectId(customer_id)
                    }

                    self.db.auctions.update({"_id": ObjectId(auction_id)}, {"$push": {"bids": bid_doc}}, True)
                    return True
                else:
                    return False
            else:
                if value > auction["minimum_bid"]:
                    bid_doc = {
                        "value": float(value),
                        "date": datetime.datetime.utcnow(),
                        "customerID": ObjectId(customer_id)
                    }

                    self.db.auctions.update({"_id": ObjectId(auction_id)}, {"$push": {"bids": bid_doc}}, True)
                    return True
                else:
                    return False
        else:
            return False

    def end_auction(self, auction_id):
        auction = self.db.auctions.find_one(ObjectId(auction_id))
        if auction:
            self.db.auctions.update({"_id": ObjectId(auction_id)}, {"$set": {"state": "finished"}})
            return True
        else:
            return False


    def subscribe(self, auction_id, customer_id, channelURI):
        customer = self.db.customers.find_one(ObjectId(customer_id))
        auction = self.db.auctions.find_one(ObjectId(auction_id))
        if customer and auction:
            self.db.customers.update({"_id": ObjectId(customer_id)}, {"$set": {"auction": auction_id}}, True)
            self.db.customers.update({"_id": ObjectId(customer_id)}, {"$set": {"channelURI": channelURI}}, True)
            user = self.db.customers.find_one(ObjectId(customer_id))
            self.send_notification(customer_id, auction_id, 2121)
            return user
        else:
            return False

    def send_notification(self, customer_id, auction_id, bid_value):
        customer = self.db.customers.find_one(ObjectId(customer_id))

        toast = MPNSToast()
        tile = MPNSTile()

        toast.send(customer["channelURI"], {'text1': 'Nova oferta', 'text2': 'Com o valor 22232 eur'})
        toast.send(customer["channelURI"], {'text1': 'Tap this message', 'text2': 'To open application'})

        tile.send(customer["channelURI"], {'title': 'Tile title'})