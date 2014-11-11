from bottle import route, run, request, response

@route('/', method="GET")
def home():
    return "Dev Auction"

run(host='localhost', port=8081, reloader=True, server='cherrypy')