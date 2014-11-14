from bottle import route, run, request, response
import data

@route('/', method="GET")
def home():
    return "Dev Auction"


@route("/api/register", method="POST")
def register():
    name = request.params.get("name")
    email = request.params.get("email")
    password = request.params.get("password")
    result = data.create_customer(name, email, password)
    if result is False:
        response.status = 400
    else:
        return {"id": str(result)}

@route("/api/bid", method="POST")
def bid():
    customerID = request.params.get("customerID")
    value =  request.params.get("value")
    date = request.params.get("date")
    result = data.create_bid(value, date, customerID)
    if result is False:
        response.status = 400
    else:
        return result


run(host='localhost', port=8082, reloader=True, server='cherrypy')
