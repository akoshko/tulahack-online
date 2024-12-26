# Intro
Tulahack.online application source code

# Misc
MIT stuff used:
1) https://github.com/AvaloniaUtils/HyperText.Avalonia
2) https://github.com/AvaloniaCommunity/Notification.Avalonia
3) https://github.com/irihitech/Ursa.Avalonia

To build docker image
docker build . -t hackathon-dashboard-backend:v1

docker run --rm -it -v ${PWD}:/local countingup/nswag openapi2csclient /input:local/Contract/contract.yaml /namespace:Tulahack.Dtos /DateType:System.DateTime /DateTimeType:System.DateTime /GenerateContractsOutput:true /ContractsOutput:local/Dtos/Tulahack.Dtos.Generated.cs /ContractsNamespace:Tulahack.Dtos /GenerateClientClasses:false /JsonLibrary:SystemTextJson /GenerateExceptionClasses:false /ArrayType:System.Collections.Generic.IList

dotnet ef migrations add InitialCreate --context TulahackContext --output-dir Migrations --verbose

# Backend ingress annotations
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    kubernetes.io/ingress.class: nginx
    meta.helm.sh/release-name: tulahack-backend
    meta.helm.sh/release-namespace: tulahack
    nginx.ingress.kubernetes.io/configuration-snippet: |
      more_set_headers "X-Forwarded-App-Type: Tulahack BFF Service";
      more_set_headers "Cross-Origin-Resource-Policy: cross-origin";
      more_set_headers "Cross-Origin-Embedder-Policy: credentialless";
      more_set_headers "Cross-Origin-Opener-Policy unsafe-none";
    nginx.ingress.kubernetes.io/cors-allow-methods: PUT, GET, POST, OPTIONS, DELETE, PATCH
    nginx.ingress.kubernetes.io/cors-allow-origin: http://localhost:4200, http://localhost:5000,
      https://localhost:5001, http://localhost:8889
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/proxy-buffer-size: 64k
    nginx.ingress.kubernetes.io/proxy-buffers-number: "8"

...
```

# Callback Backend ingress annotations
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    kubernetes.io/ingress.class: nginx
    meta.helm.sh/release-name: tulahack-backend
    meta.helm.sh/release-namespace: tulahack
    nginx.ingress.kubernetes.io/auth-response-headers: x-auth-request-user, x-auth-request-email,
      authorization
    nginx.ingress.kubernetes.io/auth-signin: https://$host/oauth2/start?rd=%2Fapi%2Fcallback  <---- rd=<encoded API callback endpoint which registers new user and then redirects to index.html>
    nginx.ingress.kubernetes.io/auth-url: http://oauth2-proxy.tulahack.svc.cluster.local/oauth2/auth
    nginx.ingress.kubernetes.io/configuration-snippet: |
      more_set_headers "X-Forwarded-App-Type: Tulahack Callback Service";
      more_set_headers "Cross-Origin-Resource-Policy: cross-origin";
      more_set_headers "Cross-Origin-Embedder-Policy: credentialless";
      more_set_headers "Cross-Origin-Opener-Policy unsafe-none";
      proxy_set_header 'X-Forwarded-Uri' $request_uri;
      auth_request_set $name_upstream_1 $upstream_cookie__oauth2_proxy_1;
      access_by_lua_block {
        if ngx.var.name_upstream_1 ~= "" then
          ngx.header["Set-Cookie"] = "_oauth2_proxy_1=" .. ngx.var.name_upstream_1 .. ngx.var.auth_cookie:match("(; .*)")
        end
      }
    nginx.ingress.kubernetes.io/cors-allow-methods: PUT, GET, POST, OPTIONS, DELETE, PATCH
    nginx.ingress.kubernetes.io/cors-allow-origin: http://localhost:4200, http://localhost:5000,
      https://localhost:5001, http://localhost:8889
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/proxy-buffer-size: 64k
    nginx.ingress.kubernetes.io/proxy-buffers-number: "8"
...
spec:
  rules:
    - host: tulahack.<you-name-it>.online
      http:
        paths:
          - backend:
              service:
                name: tulahack-backend
                port:
                  number: 80
            path: /api/callback
            pathType: ImplementationSpecific
  tls:
    - hosts:
        - tulahack.<you-name-it>.online
      secretName: tulahack-tls
...
```

# Frontend ingress annotations
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    kubernetes.io/ingress.class: nginx
    meta.helm.sh/release-name: tulahack-frontend
    meta.helm.sh/release-namespace: tulahack
    nginx.ingress.kubernetes.io/auth-response-headers: x-auth-request-user, x-auth-request-email,
      authorization
    nginx.ingress.kubernetes.io/auth-signin: https://$host/oauth2/start?rd=%2Fapi%2Fcallback  <---- rd=<encoded API callback endpoint which registers new user and then redirects to index.html>
    nginx.ingress.kubernetes.io/auth-url: http://oauth2-proxy.tulahack.svc.cluster.local/oauth2/auth
    nginx.ingress.kubernetes.io/configuration-snippet: |
      more_set_headers "X-Forwarded-App-Type: Tulahack WASM Clinet";
      proxy_set_header 'X-Forwarded-Uri' $request_uri;
      auth_request_set $name_upstream_1 $upstream_cookie__oauth2_proxy_1;
      access_by_lua_block {
        if ngx.var.name_upstream_1 ~= "" then
          ngx.header["Set-Cookie"] = "_oauth2_proxy_1=" .. ngx.var.name_upstream_1 .. ngx.var.auth_cookie:match("(; .*)")
        end
      }
    nginx.ingress.kubernetes.io/cors-allow-methods: PUT, GET, POST, OPTIONS, DELETE
    nginx.ingress.kubernetes.io/cors-allow-origin: http://localhos:4200, http://localhost:8889
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/proxy-buffer-size: 64k
    nginx.ingress.kubernetes.io/proxy-buffers-number: "8"
  labels:
    app.kubernetes.io/instance: tulahack-frontend
    app.kubernetes.io/managed-by: Helm
    app.kubernetes.io/name: hackathon-dashboard-frontend
    app.kubernetes.io/version: 1.16.0
    helm.sh/chart: hackathon-dashboard-frontend-0.1.0
  name: tulahack-frontend-hackathon-dashboard-frontend
  namespace: tulahack
```

# OAuth2 proxy ingress annotations:
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    meta.helm.sh/release-name: oauth2-proxy
    meta.helm.sh/release-namespace: tulahack
    nginx.ingress.kubernetes.io/auth-response-headers: x-auth-request-user, x-auth-request-email,
      authorization
    nginx.ingress.kubernetes.io/configuration-snippet: |
      proxy_set_header 'X-Forwarded-Uri' $request_uri;
    nginx.ingress.kubernetes.io/cors-allow-methods: PUT, GET, POST, OPTIONS, DELETE
    nginx.ingress.kubernetes.io/cors-allow-origin: http://localhost:4200, http://localhost:8889, http://localhost:5000
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/proxy-buffer-size: 8k
    nginx.ingress.kubernetes.io/proxy-buffers-number: "4"
  labels:
    app: oauth2-proxy
    app.kubernetes.io/component: authentication-proxy
    app.kubernetes.io/instance: oauth2-proxy
    app.kubernetes.io/managed-by: Helm
    app.kubernetes.io/name: oauth2-proxy
    app.kubernetes.io/part-of: oauth2-proxy
    app.kubernetes.io/version: 7.6.0
    helm.sh/chart: oauth2-proxy-7.7.1
  name: oauth2-proxy
  namespace: tulahack
...
spec:
  ingressClassName: nginx
  rules:
    - host: tulahack.<you-name-it>.online
      http:
        paths:
          - backend:
              service:
                name: oauth2-proxy
                port:
                  number: 80
            path: /oauth2
            pathType: ImplementationSpecific
  tls:
    - hosts:
        - tulahack.<you-name-it>.online
      secretName: oauth2-proxy-tls
```

# oauth2-proxy config file
```shell
  configFile: |-
    reverse_proxy = "true"

    provider = "keycloak-oidc"
    client_id = "tulahack-client"
    client_secret = "keycloak client secret"
    oidc_issuer_url = "https://keycloak.<you-name-it>.online/realms/tulahack"

    pass_access_token = "true"
    pass_authorization_header = "true"
    pass_user_headers = "true"
    set_xauthrequest = "true"
    set_authorization_header = "true"

    code_challenge_method = "S256"

    cookie_domains= [ "tulahack.<you-name-it>.online" ]
    cookie_secure = "true"
    cookie_secret = "result of <openssl rand -base64 32 | tr -- '+/' '-_'>"
    cookie_refresh= "1m"
    cookie_expire = "10m"

    email_domains = "*"
    insecure_oidc_allow_unverified_email = "true"
    whitelist_domains = [ "tulahack.<you-name-it>.online" ]

    upstreams = [ "https://tulahack.<you-name-it>.online" ]
```
