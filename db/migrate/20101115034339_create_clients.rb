class CreateClients < ActiveRecord::Migration
  def self.up
    create_table :clients do |t|
      t.string :alias, :null => false
      t.string :ip, :null => false
      t.integer :status_interval, :default => 0

      t.timestamps
    end
  end

  def self.down
    drop_table :clients
  end
end
