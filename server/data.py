from pymongo import MongoClient, errors
from bson.objectid import ObjectId
import helpers

DATA_BASE = "auction"
db = MongoClient()[DATA_BASE]


def drop_data_base():
    MongoClient().drop_database(DATA_BASE)


def create_customer(name, email, password):
    db.customers.ensure_index("email", unique=True)
    if not helpers.email_is_valid(email) or not helpers.name_is_valid(name) or not helpers.password_is_valid(password):
        return False
    encrypted_password = helpers.encrypt_password(password)
    customer_doc = {
        "name": name,
        "email": email,
        "password": encrypted_password
    }
    try:
        result = db.customers.insert(customer_doc)
    except errors.DuplicateKeyError:
        result = False
    return result


def get_customer(customer_id):
    return db.customers.find_one(ObjectId(customer_id))


def create_bid(value, date, customer_id):
    bid_doc = {
        "value": float(value),
        "date": helpers.parse_date(date, "%d/%m/%Y"),
        "customerID": ObjectId(customer_id)
    }

    bidID = db.bids.insert(bid_doc)
    return {"id": str(bidID)}
