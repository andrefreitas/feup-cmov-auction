from bottle import route, run, request, response, Bottle
import data

app = Bottle()

@app.route("/api/customers", method="POST")
def create_customer():
    name = request.params.get("name")
    email = request.params.get("email")
    password = request.params.get("password")
    result = data.create_customer(name, email, password)
    if result is False:
        response.status = 400
    else:
        return {"id": str(result)}


@app.route("/api/bid", method="POST")
def create_bid():
    customer_id = request.params.get("customerID")
    value = request.params.get("value")
    date = request.params.get("date")
    result = data.create_bid(value, date, customer_id)
    if result is False:
        response.status = 400
    else:
        return result


if __name__ == '__main__':
    run(app, host='localhost', port=8082, reloader=True, server='cherrypy')
