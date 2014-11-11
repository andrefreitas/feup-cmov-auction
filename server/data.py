from pymongo import MongoClient
db = MongoClient()['auction']
DATA_BASE = "auction"


def drop_data_base():
    MongoClient().drop_database('auction')

