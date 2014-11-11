import unittest
import requests
import data

URL = "http://localhost:8082/api"


class TestApi(unittest.TestCase):
    def setUp(self):
        data.drop_data_base()

    def test_register(self):
        payload = {
            "name": "Carlos Andrade",
            "email": "carlos@fe.up.pt",
            "password": "521ku7L7eS8Y"
        }

        # Valid registration
        answer = requests.post(URL + "/register", params=payload)
        self.assertEqual(answer.status_code, 200)

        # Invalid registration
        answer = requests.post(URL + "/register", params=payload)
        self.assertEqual(answer.status_code, 400)

        # Clear
        data.drop_data_base()



if __name__ == '__main__':
    unittest.main()