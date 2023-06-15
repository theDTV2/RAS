import requests
import datetime

# Register first door

doorID = "TestD3"
cardCode = "12345623"

# data_register = {'doorID': doorID,
#                 'timeStamp': datetime.datetime.now()}
# ret_r = requests.post("https://localhost:7073/api/Register", data=data_register, verify=False)

# print(ret_r.json())

# secret = ret_r.json()['secret']
secret = '+cNv/W3ckoI4h9Bm00ijw/yU+rIN9FxaMg8UiR0lURIWw2FJRJocdYPniccfjxnEEtB7qvYFm' \
         'NNnT98iboEki8wJ9nzUpmRN8YMHXuZ+GT1296dlpxEobALJwoKKAK1mScJCJs7tpOC2uZxNCpiBnd+p1NKoko5ywBzgJIp6U68='
# Register first door

data_heart = {'doorID': doorID,
              'timeStamp': datetime.datetime.now(),
              'secret': secret}

ret_h = requests.post("https://localhost:7073/api/Hearbeat", data=data_heart, verify=False)
print(ret_h.json())

data_request_access = {'doorID': doorID,
                       'timeStamp': datetime.datetime.now(),
                       'cardCode': cardCode,
                       'secret': secret}

ret_a = requests.post("https://localhost:7073/api/Access", data=data_request_access, verify=False)
print(ret_a.json())
