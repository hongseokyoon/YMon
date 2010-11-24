class CreateClients < ActiveRecord::Migration
  def self.up
    create_table :clients do |t|
      t.string    :alias
      t.string    :ip
      t.datetime  :time

      t.timestamps
    end
  end

  def self.down
    drop_table :clients
  end
end
