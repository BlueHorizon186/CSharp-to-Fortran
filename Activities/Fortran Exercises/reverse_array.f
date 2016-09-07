      program reverse_array
      parameter (arr_size = 5)
      real a(arr_size)

      write(*, *) 'Enter the array elements:\n'
        
      do 20 i = 1, arr_size
        read(*, *) a(i)
20    continue

      write(*, *) 'The reversed array is: '
      do 30 j = arr_size, 1, -1
        write(*, *) a(j)
30    continue

      stop
      end
