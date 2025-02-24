# Build stage
FROM node:22-alpine as build
WORKDIR /app

# Install pnpm
RUN corepack enable && corepack prepare pnpm@latest --activate

# Create directory structure
COPY . /app/

# Install dependencies
RUN pnpm install --frozen-lockfile

# Build the application
RUN pnpm run build

# Serve stage
FROM nginx:alpine

# Remove default nginx static assets
RUN rm -rf /usr/share/nginx/html/*
RUN rm /etc/nginx/conf.d/default.conf

# Copy our custom configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy the built application - Note we're copying everything from browser directory
COPY --from=build /app/dist/postcode-ui/browser/ /usr/share/nginx/html/

# Ensure proper permissions
RUN chown -R nginx:nginx /usr/share/nginx/html && \
    chmod -R 755 /usr/share/nginx/html && \
    find /usr/share/nginx/html -type f -name "*.css" -exec chmod 644 {} \; && \
    find /usr/share/nginx/html -type f -name "*.js" -exec chmod 644 {} \;

EXPOSE 80