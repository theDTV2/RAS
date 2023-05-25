import requests
import datetime

_data = {'doorID': "Test",
         'timeStamp': datetime.datetime.now()}

requests.post("https://localhost:7073/api/Register",data=_data, verify=False)
