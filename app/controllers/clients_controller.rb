class ClientsController < ApplicationController
  # GET /clients
  # GET /clients.xml
  def index
    @clients = Client.all(:order => "alias")

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
=begin  
    #render :text => params.inspect
    #render :text => "#{DateTime.now.to_s}"
    #render :inline => "#{DateTime.now.to_s}<br /><%= image_tag\(\"#{params["image"]}\", :size => \"500x300\"\) %>"
    
    clients = Client.all
    
    #render :text => "#{cpu_usage}"
    render :update do |page|
      page.replace_html 'localhost_cpu', :text => "#{DateTime.now.to_s}"
      #page.replace_html 'localhost_mem', :text => "#{avail_mem}"
      #clients.each do |client|
      #  page.replace_html "#{clients.alias}_mem", :text => "#{DateTime.now.to_s}"
      #end
    end
=end    
    client  = Client.find_by_id(params["client_id"])
    
    statuses  = client.statuses.last(21)
    
    cpu_string  = "["
    mem_string  = "["
    statuses.each do |s|
      cpu_string  += s.cpu.to_s
      cpu_string  += ","
      
      mem_string  += s.mem.to_s
      mem_string  += ","
    end
    cpu_string = cpu_string[0..cpu_string.length - 1] + "]"
    mem_string = mem_string[0..mem_string.length - 1] + "]"
    
    #render :text => "#{DateTime.now.to_s}"
    #render :partial => "cpu_usage", :object => data_string
#=begin    
    render :update do |page|
      page.replace_html 'cpu_usage1', :partial => "cpu_usage", :object => {:canvas_id => "cpu_line", :data => cpu_string, :ymin => 0, :ymax => 100, :ylabel => "CPU Usage (%)"}
      page.replace_html 'cpu_usage2', :partial => "cpu_usage", :object => {:canvas_id => "mem_line", :data => mem_string, :ymin => 0, :ymax => 2048, :ylabel => "Available Memory (MB)"}
    end
#=end
  end
end
