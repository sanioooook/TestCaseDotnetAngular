ARG API_URL
# Stage 1: Build the Angular application
FROM node:18 AS build

WORKDIR /app

# Copy package.json and install dependencies
COPY package.json ./
RUN npm install

# Copy the rest of the application code and build it
COPY . .
RUN npm run build --production

# Stage 2: Serve the Angular application with Nginx
FROM nginx:alpine

# Copy built Angular files to Nginx default public folder
COPY --from=build /app/dist/front /usr/share/nginx/html

# Expose port 80 for serving the application
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
