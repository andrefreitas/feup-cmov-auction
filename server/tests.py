from webtest import TestApp
import unittest
import data
import app
import helpers


class TestApi(unittest.TestCase):
    def setUp(self):
        self.app = TestApp(app.app)
        self.db = app.db
        self.customer1_id = self.db.create_customer("Pedro Salgado", "pedro@gmail.com", "h5hs!?dfj4")

    def tearDown(self):
        self.db.drop_data_base()

    def test_create_customer(self):
        customer_doc = {
            "name": "Carlos Andrade",
            "email": "carlos@fe.up.pt",
            "password": "521ku7L7eS8Y"
        }

        # Valid Registration
        answer = self.app.post("/api/customers",  params=customer_doc)
        self.assertEqual(answer.status_int, 200)

        # Check that was saved correctly
        customer_id = answer.json["id"]
        customer = self.db.get_customer(customer_id)
        self.assertEqual(customer["name"], customer_doc["name"])
        self.assertEqual(customer["email"], customer_doc["email"])
        self.assertEqual(customer["password"], helpers.encrypt_password(customer_doc["password"]))

        # Registration with same email
        answer = self.app.post("/api/customers", params=customer_doc, expect_errors=True)
        self.assertEqual(answer.status_int, 400)

         # Registration with invalid email
        customer_doc = {
            "name": "Carlos Andrade",
            "email": "carlosfe.up.pt",
            "password": "521ku7L7eS8Y"
        }
        answer = self.app.post("/api/customers", params=customer_doc, expect_errors=True)
        self.assertEqual(answer.status_int, 400)

    def test_login_customer(self):
        # Valid login
        login_doc = {
            "email": "pedro@gmail.com",
            "password": "h5hs!?dfj4"
        }
        answer = self.app.post("/api/login", params=login_doc)
        self.assertEqual(answer.status_int, 200)
        self.assertEqual(answer.json["id"], str(self.customer1_id))

        # Invalid Login
        login_doc = {
            "email": "pedro@gmail.com",
            "password": "1223"
        }
        answer = self.app.post("/api/login", params=login_doc, expect_errors=True)
        self.assertEqual(answer.status_int, 400)
"""
    def test_create_auction(self):
        photo = open('images/monalisa.jpeg', 'rb').read()

        auction_doc = {
            "description": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        answer = self.app.post("/api/auctions", params=auction_doc)
        self.assertEqual(answer.status_int, 200)

"""



if __name__ == '__main__':
    unittest.main()