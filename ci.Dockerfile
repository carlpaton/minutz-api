FROM node
RUN mkdir /src
WORKDIR /src
COPY ./src/minutz.ui/ /src
EXPOSE 5555
RUN npm install gulp
RUN npm install nodemon
#CMD docker run -it --rm -v %cd%/src/minutz.ui:/src node bash --this works
RUN npm install
#RUN npm run build.prod
CMD ["npm", "start"]