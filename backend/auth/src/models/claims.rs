use serde::{Deserialize, Serialize};
use chrono::{Utc, Duration};

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct Claims {
    pub sub: String,
    pub email: String,
    pub exp: usize,
}

impl Claims {
    pub fn new(user_id: String, email: String) -> Self {
        let exp = (Utc::now() + Duration::hours(24)).timestamp() as usize;
        Claims {
            sub: user_id,
            email,
            exp,
        }
    }
}