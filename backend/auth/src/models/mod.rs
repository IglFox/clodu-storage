pub mod user;
pub mod claims;

pub use user::*;
pub use claims::*;

use serde::{Deserialize, Serialize};

#[derive(Debug, Deserialize)]
pub struct RegisterRequest {
    pub email: String,
    pub password: String,
}

#[derive(Debug, Deserialize)]
pub struct LoginRequest {
    pub email: String,
    pub password: String,
}

#[derive(Debug, Serialize)]
pub struct TokenResponse {
    pub access_token: String,
    pub token_type: String,
}

impl TokenResponse {
    pub fn new(token: String) -> Self {
        TokenResponse {
            access_token: token,
            token_type: "bearer".to_string(),
        }
    }
}