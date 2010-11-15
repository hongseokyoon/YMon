# Be sure to restart your server when you modify this file.

# Your secret key for verifying cookie session data integrity.
# If you change this key, all old sessions will become invalid!
# Make sure the secret is at least 30 characters and all random, 
# no regular words or you'll be exposed to dictionary attacks.
ActionController::Base.session = {
  :key         => '_YMon_session',
  :secret      => 'ed61c18f7f2b46f531d12399227cf9ddc21d1e4ecee7e42b5a3fecc01efe2cfc4472330c17281f59bb7304ffdecad715ae5c753107357ae6227c79d6f8468fce'
}

# Use the database for sessions instead of the cookie-based default,
# which shouldn't be used to store highly confidential information
# (create the session table with "rake db:sessions:create")
# ActionController::Base.session_store = :active_record_store
