from webtest import TestApp
import unittest
import data
import app
import helpers


class TestApi(unittest.TestCase):
    def setUp(self):
        self.app = TestApp(app.app)
        data.drop_data_base()

    def test_create_customer(self):
        customer_doc = {
            "name": "Carlos Andrade",
            "email": "carlos@fe.up.pt",
            "password": "521ku7L7eS8Y"
        }

        # Valid Registration
        answer = self.app.post("/api/customers", customer_doc)
        self.assertEqual(answer.status_int, 200)

        # Check that was saved correctly
        customer_id = answer.json["id"]
        customer = data.get_customer(customer_id)
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



    def tearDown(self):
        data.drop_data_base()

if __name__ == '__main__':
    unittest.main()