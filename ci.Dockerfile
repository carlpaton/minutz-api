FROM node:8-alpine
CMD docker run -it --rm -v %cd%/src/minutz.ui:/src node bash --this works
WORKDIR /src
RUN yarn install
RUN npm run