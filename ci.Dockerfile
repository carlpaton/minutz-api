FROM node:8-alpine
VOLUME . /build
WORKDIR /build
#CMD docker run -it --rm -v %cd%/src/minutz.ui:/src node bash --this works
RUN yarn install
RUN npm run build.prod