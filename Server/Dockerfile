FROM node:12

ENV PORT 3000

WORKDIR /usr/src/app

# A wildcard is used to ensure both package.json AND package-lock.json are copied
COPY package*.json ./

RUN npm ci
# run this for production
# npm ci --only=production

COPY . .

EXPOSE 3000

CMD [ "npm", "start" ]