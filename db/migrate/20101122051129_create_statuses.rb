class CreateStatuses < ActiveRecord::Migration
  def self.up
    create_table :statuses do |t|
      t.integer :client_id
      t.integer :cpu
      t.integer :mem
      t.datetime :time

      t.timestamps
    end
  end

  def self.down
    drop_table :statuses
  end
end
