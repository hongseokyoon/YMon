class Client < ActiveRecord::Base
  has_many :statuses, :dependent => :destroy
end
