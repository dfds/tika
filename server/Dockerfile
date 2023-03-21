# https://github.com/nodejs/docker-node/blob/6bc7fe5d018f2235cdcd2f7681990cee9d096497/13/alpine3.11/Dockerfile
## Node js app builder ##
FROM node:14-alpine AS Builder

WORKDIR /app

COPY package.json .
RUN npm install

COPY webpack.config.js .
COPY tsconfig.json .
COPY ./src/ ./src/

RUN npm run build


## Node js app runner ##
FROM node:14-alpine As Runner

# Install curl & expect
RUN apk add --update \
    curl expect \
    && rm -rf /var/cache/apk/*

# Install confluent cli tool
RUN apk --no-cache add ca-certificates bash curl

COPY ./confluent-bin /usr/local/bin/confluent

RUN chmod +x /usr/local/bin/confluent && confluent version

# Copy app & supporting scripts
COPY --from=Builder /app/dist/main.js /app/main.js
COPY ./login.sh /app/login.sh
COPY ./entrypoint.sh /app/entrypoint.sh


# user:1000 group: 1000
USER node


ENTRYPOINT /app/entrypoint.sh