import requests
import datetime

_data = {'doorID': "Test",
         'timeStamp': datetime.datetime.now()}

ret = requests.post("https://localhost:7073/api/Register", data=_data, verify=False)


print(ret)


