from bottle import route, run, request, response, Bottle, static_file
from bson.json_util import dumps
import data

app = Bottle()
db = data.DataBase("auction")

@app.route("/api/customers", method="POST")
def create_customer():
    response.content_type = 'application/json'
    name = request.params.get("name")
    email = request.params.get("email")
    password = request.params.get("password")
    result = db.create_customer(name, email, password)
    if result is False:
        response.status = 400
    else:
        return {"id": str(result)}


@app.route("/api/login", method="POST")
def login():
    response.content_type = 'application/json'
    email = request.params.get("email")
    password = request.params.get("password")
    customer = db.login(email, password)
    if customer:
        return {"id": str(customer["_id"])}
    else:
        response.status = 400


@app.route("/api/bid", method="POST")
def create_bid():
    response.content_type = 'application/json'
    customer_id = request.params.get("customerID")
    value = float(request.params.get("value"))
    auction_id = request.params.get("auctionID")
    result = db.create_bid(value, customer_id, auction_id)
    if result is False:
        response.status = 400
    else:
        response.status = 200

@app.route("/api/auctions", method="POST")
def create_auction():
    response.content_type = 'application/json'
    name = request.params.get("name")
    minimum_bid = int(request.params.get("minimum_bid"))
    photo = request.files.get("photo")
    auction_id = db.create_auction(name, minimum_bid, photo)
    return {"id": str(auction_id)}

@app.route("/api/auctions", method="GET")
def get_auctions():
    response.content_type = 'application/json'
    answer_json = dumps(db.get_auctions())
    return answer_json


@app.route("/api/photos/:photo_id", method="GET")
def get_photo(photo_id):
    return static_file(photo_id, root='images', mimetype='image/png')

@app.route("/api/endauctions", method="POST")
def end_auction():
    response.content_type = 'application/json'
    auctionID = request.params.get("auctionID")
    answer = db.end_auction(auctionID)
    if answer:
        response.status = 200
    else:
        response.status = 400

@app.route("/api/subscribe", method="POST")
def subscribe():
    response.content_type = 'application/json'
    auctionID = request.params.get("auctionID")
    customerID = request.params.get("customerID")
    channelURI = request.params.get("channelURI")
    answer = db.subscribe(auctionID, customerID, channelURI)
    if answer is False:
        response.status = 400
    else:
        response.status = 200
        return dumps(answer)

if __name__ == '__main__':
    run(app, host='localhost', port=8082, reloader=True, server='cherrypy')
