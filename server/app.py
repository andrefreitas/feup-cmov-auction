from bottle import route, run, request, response, Bottle
from bson.objectid import ObjectId
import data

app = Bottle()
db = data.DataBase("auction")

@app.route("/api/customers", method="POST")
def create_customer():
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
    email = request.params.get("email")
    password = request.params.get("password")
    customer = db.login(email, password)
    if customer:
        return {"id": str(customer["_id"])}
    else:
        response.status = 400


@app.route("/api/bid", method="POST")
def create_bid():
    customer_id = request.params.get("customerID")
    value = request.params.get("value")
    date = request.params.get("date")
    result = db.create_bid(value, date, customer_id)
    if result is False:
        response.status = 400
    else:
        return result

@app.route("/api/auctions", method="POST")
def create_auction():
    name = request.params.get("name")
    minimum_bid = int(request.params.get("minimum_bid"))
    photo = request.files.get("photo")
    auction_id = db.create_auction(name, minimum_bid, photo)
    return {"id": str(auction_id)}


if __name__ == '__main__':
    run(app, host='localhost', port=8082, reloader=True, server='cherrypy')
