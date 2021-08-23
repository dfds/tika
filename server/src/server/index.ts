import {ServiceAccountsInterface} from "./api/service-accounts";
import {ApiKeysInterface} from "./api/api-keys";
import {AccessControlListsInterface} from "./api/AccessControlListsInterface";
import {TopicsInterface} from "./api/TopicsInterface";
import {NotConnectedCCloudCliWrapper} from "./wrapper/notConnected/NotConnectedCCloudCliWrapper"
import {Ccloud} from "./wrapper/ccloud";
import {RequestLogger} from "./api/middleware/requestLoggerMiddleware";
import { RequestError } from "./api/middleware/requestErrorMiddleware";
import { ResponseLogger } from "./api/middleware/responseLoggerMiddleware";
const express = require("express");
const setCorrelationId = require('express-mw-correlation-id');
const promBundle = require("express-prom-bundle");
import * as promClient from 'prom-client';
import { GetConfig } from "./config";

const promMetrics = promBundle({includePath: true});

const confluentUp = new promClient.Gauge(
  {
    name: 'tika_confluent_up',
    help: 'tika_confluent_up_help',
    labelNames: ['kafkaCluster']
  }
);

const app = express();
app.use(setCorrelationId());
app.use(promMetrics);
app.use(express.json());
app.use(RequestLogger);
app.use(ResponseLogger);


var cc: CCloudCliWrapper;

const apiImplementationToUse = process.env.TIKA_API_IMPLEMENTATION || "connected";
const appConfig = GetConfig();


console.info("Using api implementation:", apiImplementationToUse);
switch (apiImplementationToUse.valueOf()) {
    case "notconnected".valueOf():
        cc = new NotConnectedCCloudCliWrapper();
        break;
    case "connected".valueOf():
        cc = new Ccloud();
        break;
    default:
        cc = new Ccloud();
}

// Healthcheck
app.get("/healthz", async (_ : any, res : any) => {
  try {
      let topics = await cc.Kafka.Topics.getTopics();
      confluentUp.set({kafkaCluster: appConfig.clusterId}, 1);
      res.sendStatus(200);
  } catch (error) {
      confluentUp.set({kafkaCluster: appConfig.clusterId}, 0);
      console.log("Health check error: ", error);
      res.sendStatus(500);
  }
});

const serviceAccountsInterface = new ServiceAccountsInterface();
serviceAccountsInterface.configureApp(
    cc.ServiceAccounts, 
    app
);

const apiKeysInterface = new ApiKeysInterface();

apiKeysInterface.configureApp(
    cc.ApiKeys,
    app
);

const accessControlListsInterface = new AccessControlListsInterface();

accessControlListsInterface.configureApp(
    cc.Kafka.AccessControlLists,
    app
);


const topicsInterface = new TopicsInterface();

topicsInterface.configureApp(
    cc.Kafka.Topics,
    app
);

app.use(RequestError);


const port = process.env.port || 3000;

app.listen(port, () => {
    console.info(`tika is listening on port ${port}...`);
});
