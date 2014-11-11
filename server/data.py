from pymongo import MongoClient
import helpers

DATA_BASE = "auction"
db = MongoClient()[DATA_BASE]
db.e


def drop_data_base():
    MongoClient().drop_database(DATA_BASE)


def create_customer(name, email, password):
    encrypted_password = helpers.encrypt_password(password)
    customer_doc = {
        "name": name,
        "email": email,
        "password": encrypted_password
    }
    return db.customers.insert(customer_doc)
