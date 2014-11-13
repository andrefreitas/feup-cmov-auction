from pymongo import MongoClient, errors
from bson.objectid import ObjectId
import helpers

DATA_BASE = "auction"
db = MongoClient()[DATA_BASE]
db.customers.ensure_index("email", unique=True)


def create_data_base():
    db = MongoClient()[DATA_BASE]
    db.customers.ensure_index("email", unique=True)

create_data_base()

def drop_data_base():
    MongoClient().drop_database(DATA_BASE)


def create_customer(name, email, password):
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

def create_bid(value, date, customerID):
    bid_doc = {
        "value": float(value),
        "date": helpers.format_date(date),
        "customerID": ObjectId(customerID)
    }

    bidID = db.bids.insert(bid_doc)
    return {"id": str(bidID)}



