FROM node:18.17.1-alpine3.18 as base

FROM base as builder

RUN mkdir /project
WORKDIR /project

COPY . .
RUN yarn
RUN yarn build:prod

FROM base as prod
WORKDIR /project
COPY --from=builder /project/dist/OnlineServer /project
RUN npm i -g http-server

CMD ["http-server"]