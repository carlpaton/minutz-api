FROM node:6
RUN mkdir /app
VOLUME ${pwd} /app
RUN cd app
RUN ls
WORKDIR /app/src/minutz.ui
RUN ls
RUN npm install
RUN npm run build.prod