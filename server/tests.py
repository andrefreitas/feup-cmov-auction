import unittest
import requests
import data

URL = "http://localhost:8082/api"


class TestApi(unittest.TestCase):
    def setUp(self):
        data.drop_data_base()
        data.create_data_base()
        self.customer1 = {
            "name": "Carlos Andrade",
            "email": "carlos@fe.up.pt",
            "password": "521ku7L7eS8Y"
        }

    def test_register(self):
        payload = self.customer1

        # Valid registration
        answer = requests.post(URL + "/register", params=payload)
        self.assertEqual(answer.status_code, 200)

        # Invalid registration
        answer = requests.post(URL + "/register", params=payload)
        self.assertEqual(answer.status_code, 400)

        # Clear
        data.drop_data_base()

    def test_bid(self):
        data.create_data_base()
        customer = requests.post(URL + "/register", params=self.customer1)

        # Create bid
        payload = {
            "value": 7.45,
            "date": "15/11/2014",
            "customerID": customer["id"]
        }

        answer = requests.post(URL + "/bid", params=payload)
        self.assertEqual(answer.status_code, 200)

        # Clear
        data.drop_data_base()




if __name__ == '__main__':
    unittest.main()