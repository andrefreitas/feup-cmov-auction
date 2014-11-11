import hashlib
import datetime
import re


def encrypt_password(password):
    return hashlib.md5(password.encode()).hexdigest()


def parse_date(date_str, date_format):
    return datetime.datetime.strptime(date_str, date_format)


def format_date(date_instance):
    return "{:%d/%m/%Y}".format(date_instance)


def name_is_valid(name):
    return len(name) >= 2


def email_is_valid(email):
    pattern = r"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)"
    return re.match(pattern, email)


def password_is_valid(password):
    return len(password) > 3