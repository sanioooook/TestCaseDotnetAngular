ARG API_URL
# Stage 1: Build the Angular application
FROM node:16 AS build

WORKDIR /app

# Copy package.json and install dependencies
COPY package.json ./
RUN npm ci

# Copy the rest of the application code and build it
COPY . .
RUN npm run build --prod

# Stage 2: Serve the Angular application with Nginx
FROM nginx:alpine

# Copy built Angular files to Nginx default public folder
COPY --from=build /app/dist/front /usr/share/nginx/html

# Expose port 80 for serving the application
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
