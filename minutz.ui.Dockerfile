FROM nginx
COPY src/minutz.ui/dist/prod /usr/share/nginx/html
COPY src/minutz.ui/src/client/assets/fonts /usr/share/nginx/html
EXPOSE 80