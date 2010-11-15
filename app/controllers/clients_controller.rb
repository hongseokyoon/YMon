class ClientsController < ApplicationController

  def index
    @clients  = Client.all(:order => :alias)
    
    respond_to do |format|
      format.html
    end
  end

  def show
    @client = Client.find(params[:id])
    
    respond_to do |format|
      format.html
    end
  end

  def new
    @client = Client.new
    
    respond_to do |format|
      format.html
    end
  end

  def edit
    @client = Client.find(params[:id])
  end

  def create
    @client = Client.new(params[:client])
    
    respond_to do |format|
      if @client.save
        #flash[:notice] = '...'
        format.html { redirect_to(@client) }
      else
        format.html { render :action => "new" }
      end
    end
  end

  def update
    @client = Client.find(param[:id])
    
    respond_to do |format|
      if @client.update_attributes(params[:client])
        #flash[:notice] = '...'
        format.html { redirect_to(@client) }
      else
        format.html { render :action => "edit" }
      end
    end
  end

  def destroy
    @client = Client.find(params[:id])
    @client.destroy
    
    respond_to do |format|
      format.html { redirect_to(clients_url) }
    end
  end

end
