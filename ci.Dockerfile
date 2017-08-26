FROM node
RUN mkdir /src
WORKDIR /src
COPY ./src/minutz.ui/ /src
EXPOSE 5555
#CMD docker run -it --rm -v %cd%/src/minutz.ui:/src node bash --this works
RUN yarn install
#RUN npm run build.prod
CMD ["npm", "start"]