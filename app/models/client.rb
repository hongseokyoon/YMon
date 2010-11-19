require 'rubygems'
require 'gruff'

class Client < ActiveRecord::Base
  has_many :statuses
  
  validates_presence_of :alias, :ip, :status_interval
  validates_uniqueness_of :alias, :ip
  validates_format_of :ip, :with => /\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}/, :message => "is improper format"

  def updateStatusGraph
    g = Gruff::Line.new
    g.title = "Statuses for #{self.alias}"
    
    g.minimum_value = 0
    g.maximum_value = 100
    
    g.y_axis_label  = "CPU Usage(%)"
    
    #g.hide_line_number  = true
    #g.hide_title        = true
    #g.hide_legend       = true
    
    g.data("#{self.alias}", (statuses.collect {|s| s.cpu}).last(20))
    
    g.write("public/images/#{self.alias}.png")
  end
end
