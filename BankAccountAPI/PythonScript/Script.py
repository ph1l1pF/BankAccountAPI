#!/usr/bin/python3
import re
import hashlib
import time
import smtplib
import datetime
import sys
from dateutil.parser import parse
from fints.client import FinTS3PinTanClient

blz = sys.argv[1]
account =sys.argv[2]
kontonummer = sys.argv[3]
pin=sys.argv[4]
start_date = parse(sys.argv[5])
end_date = parse(sys.argv[6])
https_endpoint =sys.argv[7]
file_name=sys.argv[8]

# Connect to the bank
connection = FinTS3PinTanClient(blz, account, pin, https_endpoint)

# Get desired account
meinKonto = None
for konto in connection.get_sepa_accounts():
    if konto.accountnumber == kontonummer:
        meinKonto = konto

title_date = "Buchungstag"
title_sender = "Beguenstigter/Zahlungspflichtiger"
title_subject = "Verwendungszweck"
title_amount = "Betrag"

with open(file_name, "w") as myfile:
        myfile.write(title_date + ";"+title_sender+";"+title_subject+";"+title_amount+"\n")

for bewegung in connection.get_transactions(meinKonto, start_date, end_date):
    date = str(bewegung.data.get('date', ''))
    senderOrReceiver = str(bewegung.data.get('applicant_name', ''))
    subject = str(bewegung.data.get('purpose', ''))
    amount = str(bewegung.data.get('amount', '')).replace('<', '').replace('>', '').replace('EUR', '').rstrip()
    with open(file_name, "a") as myfile:
        myfile.write(date + ";"+senderOrReceiver+";"+subject+";"+amount+"\n")