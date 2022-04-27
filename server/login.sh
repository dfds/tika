#!/usr/bin/expect
spawn confluent login

expect "Email: "

send -- "$env(TIKA_CC_USER)\r"

expect "Password: "

send -- "$env(TIKA_CC_PASS)\r"

set timeout 600
expect eof

send_user "Confluent cloud login successful\n"