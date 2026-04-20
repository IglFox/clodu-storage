mod db;
mod error;
mod handlers;
mod models;

use axum::{
    routing::{get, post},
    Router,
};
use std::net::SocketAddr;
use tower_http::cors::{Any, CorsLayer};
use tower_http::trace::TraceLayer;

use handlers::{login, register};
use db::init_db;

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    dotenvy::dotenv().ok();

    tracing_subscriber::fmt()
        .with_env_filter("auth_service=debug,tower_http=debug")
        .init();

    let db = init_db().await?;

    let cors = CorsLayer::new()
        .allow_origin(Any)
        .allow_methods(Any)
        .allow_headers(Any);

    let public_routes = Router::new()
        .route("/auth/register", post(register))
        .route("/auth/login", post(login));

    let protected_routes = Router::new()
        .route("/auth/me", get(handlers::me))
        .layer(axum::middleware::from_fn_with_state(
            db.clone(),
            handlers::auth_middleware,
        ));

    let app = Router::new()
        .merge(public_routes)
        .merge(protected_routes)
        .route("/health", get(|| async { "OK" }))
        .layer(TraceLayer::new_for_http())
        .layer(cors)
        .with_state(db);

    let addr = SocketAddr::from(([0, 0, 0, 0], 8081));
    tracing::info!("Auth service listening on {}", addr);

    let listener = tokio::net::TcpListener::bind(addr).await?;
    // В axum 0.8 Server заменён на serve
    axum::serve(listener, app).await?;

    Ok(())
}