from pymongo import MongoClient, errors
from bson.objectid import ObjectId
import helpers
import gridfs


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
            "photo_id": photo_id
        }
        return self.db.auctions.insert(auction_doc)

    def create_bid(self, value, date, customer_id):
        bid_doc = {
            "value": float(value),
            "date": helpers.parse_date(date, "%d/%m/%Y"),
            "customerID": ObjectId(customer_id)
        }

        bid_id = self.db.bids.insert(bid_doc)
        return {"id": str(bid_id)}
