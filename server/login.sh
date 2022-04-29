#!/bin/ash
export CONFLUENT_CLOUD_EMAIL="$TIKA_CC_USER"
export CONFLUENT_CLOUD_PASSWORD="$TIKA_CC_PASS"
confluent login --save
