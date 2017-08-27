FROM tzatziki/angular:1.1
RUN mkdir /app
VOLUME . /app
WORKDIR /app/src/minutz.ui
RUN npm install
RUN npm run build.prod