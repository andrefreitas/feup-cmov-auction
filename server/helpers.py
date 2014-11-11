import hashlib
import datetime


def encrypt_password(password):
    return hashlib.md5(password.encode()).hexdigest()

def parse_date(date_str, date_format):
    return datetime.datetime.strptime(date_str, date_format)


def format_date(date_instance):
    return "{:%d/%m/%Y}".format(date_instance)