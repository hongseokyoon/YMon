class ClientsController < ApplicationController
  # GET /clients
  # GET /clients.xml
  def index
    @clients = Client.all

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :xml => @clients }
    end
  end

  # GET /clients/1
  # GET /clients/1.xml
  def show
    @client = Client.find(params[:id])

    respond_to do |format|
      format.html # show.html.erb
      format.xml  { render :xml => @client }
    end
  end

  # GET /clients/new
  # GET /clients/new.xml
  def new
    @client = Client.new

    respond_to do |format|
      format.html # new.html.erb
      format.xml  { render :xml => @client }
    end
  end

  # GET /clients/1/edit
  def edit
    @client = Client.find(params[:id])
  end

  # POST /clients
  # POST /clients.xml
  def create
    @client = Client.new(params[:client])

    respond_to do |format|
      if @client.save
        format.html { redirect_to(@client, :notice => 'Client was successfully created.') }
        format.xml  { render :xml => @client, :status => :created, :location => @client }
      else
        format.html { render :action => "new" }
        format.xml  { render :xml => @client.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /clients/1
  # PUT /clients/1.xml
  def update
    @client = Client.find(params[:id])

    respond_to do |format|
      if @client.update_attributes(params[:client])
        format.html { redirect_to(@client, :notice => 'Client was successfully updated.') }
        format.xml  { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @client.errors, :status => :unprocessable_entity }
      end
    end
  end

  # DELETE /clients/1
  # DELETE /clients/1.xml
  def destroy
    @client = Client.find(params[:id])
    @client.destroy

    respond_to do |format|
      format.html { redirect_to(clients_url) }
      format.xml  { head :ok }
    end
  end
  
  # POST /update_status
  def update_status
    client  = Client.find_by_ip(request.remote_ip.to_s)

    if (client == nil)
      render :text => "#{request.remote_ip} is not permitted."
      return
    end
    
    ymon  = params["ymon"]    
    
    # status
    unless ymon["status"].kind_of? Array
      ymon["status"]  = [ymon["status"]]
    end
    
    ymon["status"].each do |status|
      Status.create(:client_id => client.id, 
        :cpu => status["cpu"], :mem => status["mem"], :time => status["time"])
    end
    
    if (client.statuses.length > 100)
      Status.delete_all(["client_id == ? and created_at < ?", client.id, 1.hour.ago])
    end
    
    #client.updateStatusGraph
    render :text => "ok"

  end
  
  # GET /update_graph
  def update_graph
    client    = Client.find_by_id(params["client_id"])    
    statuses  = client.statuses.last(21)
    
    cpu_data  = "["
    mem_data  = "["
    statuses.each do |s|
      cpu_data  += s.cpu.to_s
      cpu_data  += ","
      
      mem_data  += s.mem.to_s
      mem_data  += ","
    end
    cpu_data = cpu_data[0..cpu_data.length - 1] + "]"
    mem_data = mem_data[0..mem_data.length - 1] + "]"
    
    render :partial => "graph", 
      :collection => [{:canvas_id => "cpu_usage_graph", 
        :data => cpu_data, :ymin => 0, :ymax => 100, :ylabel => "CPU Usage (%)"},
        {:canvas_id => "available_mem_graph", 
          :data => mem_data, :ymin => 0, :ymax => 2048, :ylabel => "Available Memory (MB)"}]
    return    
  end
end
