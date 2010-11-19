class Client < ActiveRecord::Base
  has_many :statuses
  
  validates_presence_of :alias, :ip, :status_interval
  validates_uniqueness_of :alias, :ip
  validates_format_of :ip, :with => /\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}/, :message => "is improper format"
end
