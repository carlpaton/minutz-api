FROM library/node
RUN apt-get update && apt-get install -y inotify-tools
WORKDIR /src
COPY package.json /src
RUN npm config set strict-ssl false
RUN ["/bin/bash","-c", "npm install -g gulp;npm install;"]
CMD ["npm", "start"]