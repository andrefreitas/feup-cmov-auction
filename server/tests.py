from webtest import TestApp, Upload
import unittest
import app
import helpers
import datetime


class TestApi(unittest.TestCase):
    def setUp(self):
        self.app = TestApp(app.app)
        self.db = app.db
        self.db.drop_data_base()
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
        #self.assertEqual(answer.status_int, 400)

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

    def test_create_auction(self):
        photo = Upload('images/monalisa.jpeg')
        auction_doc = {
            "name": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        # Test response status
        answer = self.app.post("/api/auctions", params=auction_doc)
        self.assertEqual(answer.status_int, 200)

        # Check that was saved correctly
        auction_id = answer.json["id"]
        auction_saved = self.db.get_auction(auction_id)
        photo_saved = Upload('images/' + auction_saved["photo_id"])
        now = datetime.datetime.now()

        self.assertEqual(auction_doc["name"], auction_saved["name"])
        self.assertEqual(now.day, auction_saved["date"].day)
        self.assertEqual(now.month, auction_saved["date"].month)
        self.assertEqual(now.year, auction_saved["date"].year)
        self.assertEqual("open", auction_saved["state"])
        self.assertEqual(int(auction_doc["minimum_bid"]), auction_saved["minimum_bid"])
        self.assertEqual(photo.content, photo_saved.content)

    def test_get_auctions(self):
        photo = Upload('images/monalisa.jpeg')
        auction_doc = {
            "name": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        self.app.post("/api/auctions", params=auction_doc)

        photo = Upload('images/thescream.jpeg')
        auction_doc = {
            "name": "The Scream",
            "minimum_bid": "10000",
            "photo": photo
        }
        self.app.post("/api/auctions", params=auction_doc)

        # Test that the auctions are retrieved
        auctions = self.app.get("/api/auctions")
        now = helpers.format_date(datetime.datetime.now())
        self.assertTrue(len(auctions.json) >= 2)
        results1 = list(filter(lambda auction: auction["name"] == "Quadro Mona Lisa" and auction["minimum_bid"] == 4230 and "photo_id" in auction and auction["date"] == now and "id" in auction, auctions.json))
        self.assertTrue(len(results1) == 1)
        results = list(filter(lambda auction: auction["name"] == "The Scream" and auction["minimum_bid"] == 10000 and "photo_id" in auction and auction["date"] == now and "id" in auction, auctions.json))
        self.assertTrue(len(results) == 1)

        #Test that the image from Mona Lisa can be retrieved
        photo_id = results1[0]["photo_id"]
        result = self.app.get("/api/photos/" + photo_id)
        image_bytes = open('images/monalisa.jpeg', "rb").read()
        self.assertTrue(result.body == image_bytes)

    def test_bid(self):
        photo = Upload('images/monalisa.jpeg')
        auction_doc = {
            "name": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        answer = self.app.post("/api/auctions", params=auction_doc)
        auction_id = answer.json["id"]

        bid_doc = {
            "value": "4000",
            "customerID": self.customer1_id,
            "auctionID": auction_id
        }

        # Test that value of bid cannot be under minimum bid
        answer = self.app.post("/api/bid", params=bid_doc, expect_errors=True)
        self.assertEqual(answer.status_int, 400)

        # Test bids ok
        bid_doc["value"] = 4500
        answer = self.app.post("/api/bid", params=bid_doc)
        self.assertEqual(answer.status_int, 200)

        bid_doc["value"] = 4700
        answer = self.app.post("/api/bid", params=bid_doc)
        self.assertEqual(answer.status_int, 200)

        # Test new bid cannot me under any of other bids
        bid_doc["value"] = 4600
        answer = self.app.post("/api/bid", params=bid_doc, expect_errors=True)
        self.assertEqual(answer.status_int, 400)

    def test_end_auction(self):
        photo = Upload('images/monalisa.jpeg')
        auction_doc = {
            "name": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        answer = self.app.post("/api/auctions", params=auction_doc)
        auction_id = answer.json["id"]

        auction =  {
            "auctionID": auction_id
        }

        # Test that state has changed
        answer = self.app.post("/api/endauctions", params=auction)
        self.assertEqual(answer.status_int, 200)

        auctions = self.app.get("/api/auctions")
        results1 = list(filter(lambda auction: auction["name"] == "Quadro Mona Lisa" and auction["minimum_bid"] == 4230 and "photo_id" in auction and auction["state"]=="finished", auctions.json))
        self.assertTrue(len(results1) == 1)

        # Test that there is no auction with that ID
        auction["auctionID"] = "123456789123456789132457"
        answer = self.app.post("/api/endauctions", params=auction, expect_errors=True)
        self.assertEqual(answer.status_int, 400)

    def test_subscribe(self):
        photo = Upload('images/monalisa.jpeg')
        auction_doc = {
            "name": "Quadro Mona Lisa",
            "minimum_bid": "4230",
            "photo": photo
        }
        answer = self.app.post("/api/auctions", params=auction_doc)
        auction_id = answer.json["id"]

        subs_doc = {
            "auctionID": auction_id,
            "customerID": self.customer1_id,
            "channelURI": "http://s.notify.live.net/u/1/db3/HmQAAAB7KCuAc5RHJDRsruvinfgOX9zZI5D-__qHSpAOP_fRTbbh_v4kaAlEJQRgA7voYbnfcdALLKYs8q99G_OT9QIb/d2luZG93c3Bob25lZGVmYXVsdA/VSw0B_umBU-5vpQAG40Omg/IJdMpgpBDrchPY9Fe_8vZEmDEoA"
        }

        answer = self.app.post("/api/subscribe", params=subs_doc)
        self.assertEqual(answer.status_int, 200)
        self.assertTrue(auction_id in answer.json["auction"])
        self.assertTrue("http://s.notify.live.net/u/1/db3/HmQAAAB7KCuAc5RHJDRsruvinfgOX9zZI5D-__qHSpAOP_fRTbbh_v4kaAlEJQRgA7voYbnfcdALLKYs8q99G_OT9QIb/d2luZG93c3Bob25lZGVmYXVsdA/VSw0B_umBU-5vpQAG40Omg/IJdMpgpBDrchPY9Fe_8vZEmDEoA" in answer.json["channelURI"])


if __name__ == '__main__':
    unittest.main()