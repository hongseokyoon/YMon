class Client < ActiveRecord::Base
  validates_uniqueness_of :alias, :ip
  validates_presence_of :alias, :ip
  validates_format_of :ip, :with => /\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/

end
